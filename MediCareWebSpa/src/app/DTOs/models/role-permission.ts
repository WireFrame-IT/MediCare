import { Permission } from "./permission";
import { Role } from "./role";

export class RolePermission {
  roleId: number;
  role: Role;
  permissionId: number;
  permission: Permission;

  constructor(
    roleId: number,
    role: Role,
    permissionId: number,
    permission: Permission
  ) {
    this.roleId = roleId;
    this.role = role;
    this.permissionId = permissionId;
    this.permission = permission;
  }
}
