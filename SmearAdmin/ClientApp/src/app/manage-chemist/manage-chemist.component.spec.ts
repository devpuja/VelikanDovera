import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageChemistComponent } from './manage-chemist.component';

describe('ManageChemistComponent', () => {
  let component: ManageChemistComponent;
  let fixture: ComponentFixture<ManageChemistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageChemistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageChemistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
