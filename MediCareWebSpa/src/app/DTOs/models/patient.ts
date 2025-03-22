import { Appointment } from "./appointment";
import { Feedback } from "./feedback";
import { User } from "./user";

export class Patient {
  userId: number;
  user: User;
  registerDate: Date;
  birthDate: Date;
  patientCard: string;
  appointments: Appointment[] = [];
  feedbacks: Feedback[] = [];

  constructor(
    userId: number,
    user: User,
    registerDate: Date,
    birthDate: Date,
    patientCard: string
  ) {
    this.userId = userId;
    this.user = user;
    this.registerDate = registerDate;
    this.birthDate = birthDate;
    this.patientCard = patientCard;
  }
}
