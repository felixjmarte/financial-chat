import { EventEmitter, Injectable } from '@angular/core';
import { AuthorizeService, IUser } from "../../api-authorization/authorize.service";
import { LogLevel, HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ChatMessageDto } from '../web-api-client';

@Injectable()
export class ChatService {
  messageReceived = new EventEmitter<ChatMessageDto>();
  connectionEstablished = new EventEmitter<Boolean>();

  private connectionIsEstablished = false;
  private _hubConnection: HubConnection;

  constructor(private authorizeService: AuthorizeService) {
    this.authorizeService.getAccessToken().subscribe((accessToken) => {
      if (accessToken) {
        this.createConnection(accessToken);
        this.registerOnServerEvents();
        this.startConnection();
      }
    })
  }

  sendMessage(message: ChatMessageDto) {
    this._hubConnection.invoke('SendMessage', message);
  }

  private createConnection(accessToken) {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl('/ChatHub', { accessTokenFactory: () => accessToken })
      .configureLogging(LogLevel.Information)
      .build();
  }

  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        this.connectionEstablished.emit(true);
      })
      .catch(err => {
        setTimeout(() => { this.startConnection(); }, 5000);
      });
  }

  private registerOnServerEvents(): void {
    this._hubConnection.on('MessageReceived', (data: ChatMessageDto) => {
      this.messageReceived.emit(data);
    });
  }
}    