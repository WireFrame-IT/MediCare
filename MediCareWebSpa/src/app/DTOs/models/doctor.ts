import { Appointment } from "./appointment";
import { Speciality } from "./speciality";
import { User } from "./user";

export class Doctor {
  id: number;
  userId: number;
  user: User;
  specialityId: number;
  speciality: Speciality;
  isAvailable: boolean;
  employmentDate: Date;
  appointments: Appointment[] = [];

  constructor(
    id: number,
    userId: number,
    user: User,
    specialityId: number,
    speciality: Speciality,
    isAvailable: boolean,
    employmentDate: Date
  ) {
    this.id = id;
    this.userId = userId
    this.user = user;
    this.specialityId = specialityId;
    this.speciality = speciality;
    this.isAvailable = isAvailable;
    this.employmentDate = employmentDate;
  }
}
