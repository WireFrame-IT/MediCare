import { RoleType } from "../../enums/role-type";

export class RolePermissionRequest {
  roleType: RoleType;
  permissionId: number;

  constructor(
    roleType: RoleType,
    permissionId: number,
  ) {
    this.roleType = roleType;
    this.permissionId = permissionId;
  }
}
