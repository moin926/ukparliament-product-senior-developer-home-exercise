import { Component } from '@angular/core';
import { PersonService } from '@services/person.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  constructor(private personService: PersonService) { }
}
