import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemistStockistResourceComponent } from './chemist-stockist-resource.component';

describe('ChemistStockistResourceComponent', () => {
  let component: ChemistStockistResourceComponent;
  let fixture: ComponentFixture<ChemistStockistResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChemistStockistResourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemistStockistResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
