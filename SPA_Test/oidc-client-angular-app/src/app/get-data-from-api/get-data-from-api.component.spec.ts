import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GetDataFromApiComponent } from './get-data-from-api.component';

describe('GetDataFromApiComponent', () => {
  let component: GetDataFromApiComponent;
  let fixture: ComponentFixture<GetDataFromApiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GetDataFromApiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GetDataFromApiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
