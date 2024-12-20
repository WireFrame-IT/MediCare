export class LoginResponseDTO {
  accessToken: string;
  refreshToken: string;
  roleType: number;

  constructor(accessToken: string, refreshToken: string, roleType: number) {
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;
    this.roleType = roleType;
  }
}
