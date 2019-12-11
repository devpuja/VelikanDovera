import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactResourceComponent } from './contact-resource.component';

describe('ContactResourceComponent', () => {
  let component: ContactResourceComponent;
  let fixture: ComponentFixture<ContactResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContactResourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContactResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
