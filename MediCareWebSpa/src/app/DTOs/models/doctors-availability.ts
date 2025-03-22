import { User } from "./user";

export class DoctorsAvailability {
  doctorsUserId: number;
  user: User;
  from: Date;
  to: Date;

  constructor(
    doctorsUserId: number,
    user: User,
    from: Date,
    to: Date
  ) {
    this.doctorsUserId = doctorsUserId
    this.user = user;
    this.from = from;
    this.to = to;
  }
}
