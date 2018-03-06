import { TestBed, inject } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable } from 'rxjs/Observable';

import { WizardEffects } from './wizard.effects';

describe('WizardService', () => {
  let actions$: Observable<any>;
  let effects: WizardEffects;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        WizardEffects,
        provideMockActions(() => actions$)
      ]
    });

    effects = TestBed.get(WizardEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});
