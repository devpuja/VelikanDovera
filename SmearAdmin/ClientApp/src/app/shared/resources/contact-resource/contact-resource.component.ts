import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-contact-resource',
  templateUrl: './contact-resource.component.html',
  styleUrls: ['./contact-resource.component.css']
})
export class ContactResourceComponent implements OnInit {
  @Input('parentForm')

  public contactForm: FormGroup;

  constructor() { }

  ngOnInit() {
  }

}
