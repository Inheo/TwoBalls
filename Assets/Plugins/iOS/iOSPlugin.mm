extern "C"
{
#import <FBAudienceNetwork/FBAudienceNetwork.h>
    void _sendFacebookData(int isEnabled)
    {
        if(isEnabled == 1)
        {
            NSLog(@"SENDING FACEBOOK DATA");
            [FBAdSettings setAdvertiserTrackingEnabled:YES];
            NSLog(@"FACEBOOK DATA HAS BEEN SENT");
        }
        else
        {
            NSLog(@"WILL NOT BE SENDING FACEBOOK FLAG");
        }
    }
     
}

