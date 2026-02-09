export interface JWTPayload {
  UserName: string;
  FullName: string;
  Id: string;
  IsAdmin: string;
}
export interface JWTPayloadConverted {
  UserName: string;
  FullName: string;
  Id: number;
  IsAdmin: boolean;
}