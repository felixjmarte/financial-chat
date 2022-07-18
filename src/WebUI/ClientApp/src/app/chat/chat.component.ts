import { Component, OnInit, NgZone, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { AuthorizeService, IUser } from "../../api-authorization/authorize.service";
import { Observable } from 'rxjs';
import { filter, map, mergeMap, take, tap } from 'rxjs/operators';
import {
  ChatRoomsClient, ChatRoomVm, ChatRoomDto,
  ChatMessagesClient, ChatMessageDto, SendChatMessageCommand
} from '../web-api-client';
import { ChatService } from './chat.service'; 

@Component({
  selector: 'app-home',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  public currentUser?: any;
  roomList: ChatRoomVm;
  currentRoom: ChatRoomDto;
  newMessage = '';
  @ViewChild('messagesContainer') messagesContainer: ElementRef;

  constructor(
    private authorizeService: AuthorizeService,
    private roomsClient: ChatRoomsClient,
    private messagesClient: ChatMessagesClient,
    private chatService: ChatService,
    private _ngZone: NgZone,
  ) {
  }

  ngOnInit(): void {
    this.authorizeService.getUser().subscribe(
      result => {
        this.currentUser = result;
      }
    )
    this.subscribeToEvents();
    this.getRooms();
  }

  ngAfterViewInit() {
    this.scrollToBottom();
  }

  scrollToBottom = () => {
    setTimeout(() => {
      try {
        this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
      } catch (err) { console.log('...'); }
    }, 1000);
    
  }

  getRooms() {
    this.roomsClient.get().subscribe(
      (result: ChatRoomVm) => {
        result.chatRooms.forEach(r => {
          r.messages = this.orderByDateDesc(r.messages)
          if (!this.currentRoom || this.currentRoom.code == r.code) {
            this.currentRoom = r;
          }
        });
        this.roomList = result;
        this.scrollToBottom();
      },
      error => console.error(error)
    );
  }

  setRoom(room) {
    if (!this.currentRoom || this.currentRoom.code != room.code) {
      this.currentRoom = room;
      this.scrollToBottom();
    }
  }

  orderByDateDesc(messages) {
    return messages.length == 0
      ? [] :
      messages.sort((a, b) => a.created - b.created);
  }

  sendMessage() {
    if (!this.currentRoom || this.newMessage.trim() === '') {
      return;
    }

    try {
      this.chatService.sendMessage({ chatRoomCode: this.currentRoom.code, message: this.newMessage } as ChatMessageDto);
      this.newMessage = '';
    } catch (err) {
      console.log(err);
    }
  }

  private subscribeToEvents(): void {
    this.chatService.messageReceived.subscribe((message: ChatMessageDto) => {
      this._ngZone.run(() => {
        message.created = new Date(message.created);
        var room = this.roomList.chatRooms.filter(r => r.code == message.chatRoomCode);
        if (room && room.length == 1) {
          room[0].messages.push(message);
          this.scrollToBottom();
        }
      });
    });
  }  
}
