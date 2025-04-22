import { PermissionType } from "../../enums/permission-type";
import { RolePermission } from "./role-permission";

export class Permission {
  id: number;
  description: string;
  doctorOnly: boolean;
  patientOnly: boolean;
  permissionType: PermissionType;
  permissionRoles: RolePermission[] = [];

  constructor(
    id: number,
    description: string,
    doctorOnly: boolean,
    patientOnly: boolean,
    permissionType: PermissionType
  ) {
    this.id = id;
    this.description = description;
    this.doctorOnly = doctorOnly;
    this.patientOnly = patientOnly;
    this.permissionType = permissionType;
  }
}
