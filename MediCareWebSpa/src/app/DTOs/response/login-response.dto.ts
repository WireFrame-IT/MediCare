export class LoginResponseDTO {
  userId: number;
  userName: string;
  userSurname: string;
  accessToken: string;
  refreshToken: string;
  roleType: number;

  constructor(
    userId: number,
    userName: string,
    userSurname: string,
    accessToken: string,
    refreshToken: string,
    roleType: number
  ) {
    this.userId = userId;
    this.userName = userName;
    this.userSurname = userSurname;
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;
    this.roleType = roleType;
  }
}
