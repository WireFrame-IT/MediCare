import { Role } from "./role";

export class User {
  id: number;
  email: string;
  name: string;
  surname: string;
  pesel: string;
  phoneNumber: string;
  roleId: number;
  role: Role;

  constructor(
    id: number,
    email: string,
    name: string,
    surname: string,
    pesel: string,
    phoneNumber: string,
    roleId: number,
    role: Role
  ) {
    this.id = id;
    this.email = email;
    this.name = name;
    this.surname = surname;
    this.pesel = pesel;
    this.phoneNumber = phoneNumber;
    this.roleId = roleId;
    this.role = role;
  }
}
