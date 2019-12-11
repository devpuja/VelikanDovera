import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'splitClaim' })
export class SplitClaim implements PipeTransform {
  transform(value: string): string {
    let newStr: string = "";
    var splitComma: any;
    splitComma = value.toString().split(",");
    
    if (splitComma.length > 0) {
      for (var i = 0; i < splitComma.length; i++) {
        newStr += splitComma[i].toString().split(".")[2] + ",";
      }

      newStr = newStr.substring(0, newStr.length - 1);
    }
    else {
      newStr = value.toString().split(".")[2];
    }
    return newStr;
  }
}
