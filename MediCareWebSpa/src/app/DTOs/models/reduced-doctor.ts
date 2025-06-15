import { DoctorsAvailability } from "./doctors-availability";
import { ReducedUser } from "./reduced-user";
import { Speciality } from "./speciality";

type TimeRange = { start: Date; end: Date };

export class ReducedDoctor {
  userId: number;
  user: ReducedUser;
  specialityId: number;
  speciality: Speciality;
  doctorsAvailabilities: DoctorsAvailability[] = [];
  unavailableTerms: TimeRange[] = [];

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
