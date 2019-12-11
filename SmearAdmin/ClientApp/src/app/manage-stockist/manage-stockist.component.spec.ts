import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageStockistComponent } from './manage-stockist.component';

describe('ManageStockistComponent', () => {
  let component: ManageStockistComponent;
  let fixture: ComponentFixture<ManageStockistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageStockistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageStockistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
