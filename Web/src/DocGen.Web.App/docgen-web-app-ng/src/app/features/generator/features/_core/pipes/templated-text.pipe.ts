import { Pipe, PipeTransform } from '@angular/core';

import { InputValueCollection, InputValueCollectionUtility } from '../../../../_core';

@Pipe({
  name: 'templatedText'
})
export class TemplatedTextPipe implements PipeTransform {

  transform(templateText: string, values: InputValueCollection): string {
    if (!templateText) {
      return '';
    }
    return InputValueCollectionUtility.getString(templateText, values);
  }

}
