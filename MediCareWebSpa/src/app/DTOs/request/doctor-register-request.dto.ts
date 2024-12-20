import { RegisterRequestDTO } from '../register-request.dto';

export class DoctorRegisterRequestDTO extends RegisterRequestDTO {
  employmentDate: string;
  specialityId: number;

  constructor(
    name: string,
    surname: string,
    email: string,
    password: string,
    pesel: string,
    phoneNumber: string,
    employmentDate: string,
    specialityId: number
  ) {
    super(name, surname, email, password, pesel, phoneNumber);
    this.employmentDate = employmentDate;
    this.specialityId = specialityId;
  }
}
