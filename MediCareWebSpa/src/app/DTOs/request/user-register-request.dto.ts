import { RoleType } from '../../enums/role-type';
import { RegisterRequestDTO } from './register-request.dto';

export class UserRegisterRequestDTO extends RegisterRequestDTO {
  employmentDate: Date | null;
  specialityId: number | null;
  birthDate: Date | null;

  constructor(
    name: string,
    surname: string,
    email: string,
    password: string,
    pesel: string,
    phoneNumber: string,
    roleType: RoleType,
    employmentDate: Date,
    specialityId: number,
    birthDate: Date
  ) {
    super(name, surname, email, password, pesel, phoneNumber, roleType);
    this.employmentDate = employmentDate;
    this.specialityId = specialityId;
    this.birthDate = birthDate;
  }
}
