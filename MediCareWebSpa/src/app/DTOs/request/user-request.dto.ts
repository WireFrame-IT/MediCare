export class UserRequestDTO {
  id: number;
  name: string;
  surname: string;
  email: string;
  password: string;
  pesel: string;
  phoneNumber: string;
  specialityId: number | null = null;
  birthDate: Date | null = null;

  constructor(
    id: number,
    name: string,
    surname: string,
    email: string,
    password: string,
    pesel: string,
    phoneNumber: string
  ) {
    this.id = id;
    this.name = name;
    this.surname = surname;
    this.email = email;
    this.password = password;
    this.pesel = pesel;
    this.phoneNumber = phoneNumber;
  }
}
