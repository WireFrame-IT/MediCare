export class RegisterRequestDTO {
  name: string;
  surname: string;
  email: string;
  password: string;
  pesel: string;
  phoneNumber: string;

  constructor(
    name: string,
    surname: string,
    email: string,
    password: string,
    pesel: string,
    phoneNumber: string
  ) {
    this.name = name;
    this.surname = surname;
    this.email = email;
    this.password = password;
    this.pesel = pesel;
    this.phoneNumber = phoneNumber;
  }
}
