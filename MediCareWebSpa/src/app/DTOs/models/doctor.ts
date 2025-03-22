import { Appointment } from "./appointment";
import { DoctorsAvailability } from "./doctors-availability";
import { Speciality } from "./speciality";
import { User } from "./user";

export class Doctor {
  userId: number;
  user: User;
  specialityId: number;
  speciality: Speciality;
  employmentDate: Date;
  appointments: Appointment[] = [];
  doctorsAvailabilities: DoctorsAvailability[] = [];

  constructor(
    userId: number,
    user: User,
    specialityId: number,
    speciality: Speciality,
    employmentDate: Date
  ) {
    this.userId = userId
    this.user = user;
    this.specialityId = specialityId;
    this.speciality = speciality;
    this.employmentDate = employmentDate;
  }
}
