import { RolePermission } from "./role-permission";

export class Permission {
  id: number;
  description: string;
  permissionRoles: RolePermission[] = [];

  constructor(
    id: number,
    description: string
  ) {
    this.id = id;
    this.description = description;
  }
}
