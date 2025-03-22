import { RoleType } from "../../enums/role-type";
import { RolePermission } from "./role-permission";

export class Role {
  id: number;
  roleType: RoleType;
  rolePermissions: RolePermission[] = [];

  constructor(
    id: number,
    roleType: RoleType
  ) {
    this.id = id;
    this.roleType = roleType;
  }
}
