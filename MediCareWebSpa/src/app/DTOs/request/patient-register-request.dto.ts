import { RoleType } from '../../enums/role-type';
import { RegisterRequestDTO } from './register-request.dto';

export class PatientRegisterRequestDTO extends RegisterRequestDTO {
  birthDate: Date;

  constructor(
    name: string,
    surname: string,
    email: string,
    password: string,
    pesel: string,
    phoneNumber: string,
    birthDate: Date
  ) {
    super(name, surname, email, password, pesel, phoneNumber, RoleType.Patient);
    this.birthDate = birthDate;
  }
}
