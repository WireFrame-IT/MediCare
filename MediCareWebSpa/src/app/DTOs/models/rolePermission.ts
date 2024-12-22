import { Permission } from "./permission";
import { Role } from "./role";

export class RolePermission {
  id: number;
  roleId: number;
  role: Role;
  permissionId: number;
  permission: Permission;

  constructor(
    id: number,
    roleId: number,
    role: Role,
    permissionId: number,
    permission: Permission
  ) {
    this.id = id;
    this.roleId = roleId;
    this.role = role;
    this.permissionId = permissionId;
    this.permission = permission;
  }
}
