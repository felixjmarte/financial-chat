import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from "../../api-authorization/authorize.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  constructor(private authorizeService: AuthorizeService) { }

   ngOnInit(): void {
  }
}
