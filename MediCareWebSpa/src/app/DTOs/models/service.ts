import { Speciality } from "./speciality";

export class Service {
  id: number;
  name: string;
  description: string;
  price: number;
  durationMinutes: number;
  specialityId: number;
  speciality: Speciality;

  constructor(
    id: number,
    name: string,
    description: string,
    price: number,
    durationMinutes: number,
    specialityId: number,
    speciality: Speciality
  ) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.price = price;
    this.durationMinutes = durationMinutes;
    this.specialityId = specialityId;
    this.speciality = speciality;
  }
}
