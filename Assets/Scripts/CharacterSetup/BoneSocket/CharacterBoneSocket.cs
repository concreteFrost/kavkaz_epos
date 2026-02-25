using UnityEngine;

[System.Serializable]
public class BoneSocket
{
    public Transform socketTransform;
    public HumanBodyBones bone;
    public Vector3 localPosition;
    public Vector3 localRotation;   

}

public class CharacterBoneSocket : MonoBehaviour
{
    [SerializeField] BoneSocket weaponSocket;
    [SerializeField] BoneSocket shieldSocket;
    [SerializeField] BoneSocket rightMeleeSocket;
    [SerializeField] BoneSocket leftMeleeSocket;
    [SerializeField] BoneSocket spellCastSocket;
    [SerializeField] BoneSocket eyesSocket;

    Animator anim;

    public void Init(Animator anim)
    {
        this.anim = anim;   

        BindSocket(weaponSocket);
        BindSocket(shieldSocket);
        BindSocket(rightMeleeSocket);
        BindSocket(leftMeleeSocket);
        BindSocket(spellCastSocket);
        BindSocket(eyesSocket);

    }

    private void BindSocket(BoneSocket socket)
    {
        if (socket.socketTransform == null) return;

        Transform bone = anim.GetBoneTransform(socket.bone);
        
        if(bone == null) return;    

        socket.socketTransform.SetParent(bone);
        socket.socketTransform.localPosition = socket.localPosition;
        socket.socketTransform.localEulerAngles = socket.localRotation;

    }

    public Transform GetWeaponHolder => weaponSocket.socketTransform;
    public Transform GetShieldHolder => shieldSocket.socketTransform;   
    public Transform GetRightMeleeSocket => rightMeleeSocket.socketTransform;
    public Transform GetLeftMeleeSocket => leftMeleeSocket.socketTransform;
    public Transform GetSpellCastSocket => spellCastSocket.socketTransform;
    public Transform GetEyesSocket => eyesSocket.socketTransform;   

  
}
