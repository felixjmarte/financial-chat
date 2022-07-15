import { EventEmitter, Injectable } from '@angular/core';
import { LogLevel, HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ChatMessageDto } from '../web-api-client';

@Injectable()
export class ChatService {
  messageReceived = new EventEmitter<ChatMessageDto>();
  connectionEstablished = new EventEmitter<Boolean>();

  private _skip = true;
  private connectionIsEstablished = false;
  private _hubConnection: HubConnection;

  constructor() {
    if (this._skip) {
      return;
    }
    //this.createConnection();
    //this.registerOnServerEvents();
    //this.startConnection();
  }

  sendMessage(message: any) {
    this._hubConnection.invoke('NewMessage', message);
  }

  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl('/chathub')
      .configureLogging(LogLevel.Information)
      .build();
  }

  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .catch(err => {
        console.log(`Error while establishing connection, retrying... ${err}`,);
        setTimeout(() => { this.startConnection(); }, 5000);
      });
  }

  private registerOnServerEvents(): void {
    this._hubConnection.on('MessageReceived', (data: any) => {
      this.messageReceived.emit(data);
    });
  }
}    