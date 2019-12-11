import { Observable } from 'rxjs/Rx';
import 'rxjs/add/observable/throw';

export abstract class BaseService {  
    
    constructor() { }

  protected handleError(error: any) {
    //debugger;
    var applicationError = error.headers.get('Application-Error');

    // either applicationError in header or model error in body
    if (applicationError) {
      return Observable.throw(applicationError);
    }

    var modelStateErrors: string = '';
    var serverError = error; //error.json();

    if (!serverError.type) {
      for (var key in serverError) {
        if (serverError[key])
          modelStateErrors += serverError[key] + '\n';
      }
    }

    modelStateErrors = modelStateErrors = '' ? '' : modelStateErrors;
    return Observable.throw(modelStateErrors || 'Server error');
  }
}
