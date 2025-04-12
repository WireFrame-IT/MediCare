export class UserRequestDTO {
  name: string;
  surname: string;
  email: string;
  newPassword: string;
  pesel: string;
  phoneNumber: string;
  specialityId: number | null = null;
  birthDate: Date | null = null;

  constructor(
    name: string,
    surname: string,
    email: string,
    newPassword: string,
    pesel: string,
    phoneNumber: string
  ) {
    this.name = name;
    this.surname = surname;
    this.email = email;
    this.newPassword = newPassword;
    this.pesel = pesel;
    this.phoneNumber = phoneNumber;
  }
}
