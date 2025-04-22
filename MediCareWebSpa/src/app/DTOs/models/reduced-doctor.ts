import { ReducedUser } from "./reduced-user";
import { Speciality } from "./speciality";

export class ReducedDoctor {
  userId: number;
  user: ReducedUser;
  specialityId: number;
  speciality: Speciality;

  constructor(
    userId: number,
    user: ReducedUser,
    specialityId: number,
    speciality: Speciality
  ) {
    this.userId = userId;
    this.user = user;
    this.specialityId = specialityId;
    this.speciality = speciality;
  }
}
