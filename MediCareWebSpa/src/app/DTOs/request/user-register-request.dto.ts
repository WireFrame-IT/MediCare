import { RoleType } from '../../enums/role-type';
import { RegisterRequestDTO } from './register-request.dto';

export class UserRegisterRequestDTO extends RegisterRequestDTO {
  employmentDate: string | null;
  specialityId: number | null;
  birthDate: string | null;

  constructor(
    name: string,
    surname: string,
    email: string,
    password: string,
    pesel: string,
    phoneNumber: string,
    roleType: RoleType,
    employmentDate: string,
    specialityId: number,
    birthDate: string
  ) {
    super(name, surname, email, password, pesel, phoneNumber, roleType);
    this.employmentDate = employmentDate;
    this.specialityId = specialityId;
    this.birthDate = birthDate;
  }
}
