import { RolePermission } from "./rolePermission";

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
