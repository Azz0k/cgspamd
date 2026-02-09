import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"
import type {JWTPayload, JWTPayloadConverted} from "@/interfaces/JWT-payload.ts";
import {jwtDecode} from "jwt-decode";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function convertJWTPayload(token: string):JWTPayloadConverted {
  const decoded = jwtDecode<JWTPayload>(token);
  return  {
    FullName: decoded.FullName,
    UserName: decoded.UserName,
    Id: parseInt(decoded.Id),
    IsAdmin: decoded.IsAdmin.toLowerCase() ==="true"
  };
}
