import { RolePermission } from "./role-permission";

export class Permission {
  id: number;
  description: string;
  doctorOnly: boolean;
  patientOnly: boolean;
  permissionRoles: RolePermission[] = [];

  constructor(
    id: number,
    description: string,
    doctorOnly: boolean,
    patientOnly: boolean
  ) {
    this.id = id;
    this.description = description;
    this.doctorOnly = doctorOnly;
    this.patientOnly = patientOnly;
  }
}
