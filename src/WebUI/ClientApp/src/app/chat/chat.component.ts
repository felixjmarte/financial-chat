import { Component, OnInit } from '@angular/core';
import { AuthorizeService, IUser } from "../../api-authorization/authorize.service";
import { Observable } from 'rxjs';
import { filter, map, mergeMap, take, tap } from 'rxjs/operators';
import {
  ChatRoomsClient, ChatRoomsVm,
  ChatMessagesClient, ChatMessageDto, SendChatMessageCommand
} from '../web-api-client';

@Component({
  selector: 'app-home',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  public currentUser?: any;
  roomList: ChatRoomsVm[];
  currentRoom: ChatRoomsVm;
  newMessage = '';

  constructor(
    private authorizeService: AuthorizeService,
    private roomsClient: ChatRoomsClient,
    private messagesClient: ChatMessagesClient,
  ) { }

  ngOnInit(): void {
    this.authorizeService.getUser().subscribe(
      result => {
        this.currentUser = result;
      }
    )
    this.roomsClient.get().subscribe(
      result => {
        result.forEach(r => {
          r.messages = this.orderByDateDesc(r.messages)
          if (!this.currentRoom && r.code) {
            this.currentRoom = r;
          }
        });
        this.roomList = result;
      },
      error => console.error(error)
    );
  }

  setRoom(room) {
    if (!this.currentRoom || this.currentRoom.code != room.code) {
      this.currentRoom = room;
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
      this.messagesClient.send({ chatRoomCode: this.currentRoom.code, message: this.newMessage } as SendChatMessageCommand).subscribe(
        result => {
          console.log(result);
        },
        error => {
          console.log(error);
        }
      );
      this.newMessage = '';
    } catch (err) {
      console.log(err);
    }
  }
}
