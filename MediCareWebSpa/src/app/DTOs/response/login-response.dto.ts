export class LoginResponseDTO {
  userName: string;
  userSurname: string;
  accessToken: string;
  refreshToken: string;
  roleType: number;

  constructor(
    userName: string,
    userSurname: string,
    accessToken: string,
    refreshToken: string,
    roleType: number
  ) {
    this.userName = userName;
    this.userSurname = userSurname;
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;
    this.roleType = roleType;
  }
}
