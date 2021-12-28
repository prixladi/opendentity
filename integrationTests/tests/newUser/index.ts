import currentUser from '../shared/currentUser';
import { newUser, newUserEmail, newUserPassword } from '../shared/utils';
import authorization from '../shared/authorization';
import createNewUser from './createNewUser';

export default () => {
  describe('NewUser', () => {
    createNewUser();
    authorization(newUser, newUserEmail, newUserPassword);
    currentUser(newUser, newUserEmail, newUserPassword);
  });
};
