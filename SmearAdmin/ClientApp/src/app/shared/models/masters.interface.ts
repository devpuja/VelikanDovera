export interface Masters {
  Id: number;
  Value: string;
  Type: string;
}


export interface MastersFor {
  value: string;
  viewValue: string;
}

export interface MastersGroup {
  //disabled?: boolean;
  name: string;
  mastersFor: MastersFor[];
}

