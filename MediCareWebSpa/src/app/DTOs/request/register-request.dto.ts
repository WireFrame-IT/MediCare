import { RoleType } from "../../enums/role-type";

export class RegisterRequestDTO {
  name: string;
  surname: string;
  email: string;
  password: string;
  pesel: string;
  phoneNumber: string;
  roleType: RoleType;

  constructor(
    name: string,
    surname: string,
    email: string,
    password: string,
    pesel: string,
    phoneNumber: string,
    roleType: RoleType
  ) {
    this.name = name;
    this.surname = surname;
    this.email = email;
    this.password = password;
    this.pesel = pesel;
    this.phoneNumber = phoneNumber;
    this.roleType = roleType;
  }
}
