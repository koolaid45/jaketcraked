namespace Jaket.UI.Dialogs;

using Steamworks.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

using Jaket.Assets;
using Jaket.Net;
using Jaket.World;

using static Pal;
using static Rect;

/// <summary> Browser for public lobbies that receives the list via Steam API and displays it in the scrollbar. </summary>
public class LobbyList : CanvasSingleton<LobbyList>
{
    /// <summary> List of lobbies currently displayed. </summary>
    public Lobby[] Lobbies;
    /// <summary> Button that updates the lobby list. </summary>
    private Button refresh;

    public string[] colortable= {"red", "lime", "blue","purple"};
    /// <summary> String by which the lobby will be searched. </summary>
    public string search = "";
    /// <summary> Content of the lobby list. </summary>
    public Lobby spamlob;
    public bool spamlobready = false;
    private RectTransform content;
    private static void spamjoin(Lobby lobby)
    {
        LobbyList lobls = new LobbyList();
        lobls.spamlobready = true;
        lobls.spamlob = lobby;
        
    }
    public static void spamjoinall()
    {
        var mc = new LobbyList();
        var lobbies = mc.search == "" ? mc.Lobbies : Array.FindAll(mc.Lobbies, lobby => lobby.GetData("name").ToLower().Contains(mc.search));
        for (int i = 0; i < 6969; i++)
        {
            foreach (var lobby in lobbies)
                LobbyController.JoinLobby(lobby);
        }
        LobbyController.LeaveLobby(false);
    }
    private void Start()
    {
        UIB.Table("List", "#lobby-list.name", transform, Size(1040f, 1040f), table =>
        {
            refresh = UIB.Button("", table, new(100f, -68f, 184f, 40f, new(0f, 1f)), clicked: Refresh);
            UIB.Field("#lobby-list.search", table, new(392f, -68f, 384f, 40f, new(0f, 1f)), cons: text =>
            {
                search = text.Trim().ToLower();
                Rebuild();
            });
            Action leavelob = () => LobbyController.LeaveLobby(true);
            UIB.IconButton("X", table, Icon(482f, 68f), red, clicked: Toggle);
            UIB.Button("KILLALL", table, new(-175f, -500f, 356f, 40f), clicked: LobbyList.spamjoinall).targetGraphic.color = UnityEngine.Color.red;
            UIB.Button("FORCE LEAVE LOBBY", table, new(175f, -500f, 356f, 40f), clicked: leavelob).targetGraphic.color = UnityEngine.Color.red;
            content = UIB.Scroll("List", table, new(0f, 272f, 1000f, 944f, new(.5f, 0f), new(.5f, 0f))).content;
        });
        Refresh();
    }

    // <summary> Toggles visibility of the lobby list. </summary>
    public void Toggle()
    {
        if (!Shown) UI.HideCentralGroup();

        gameObject.SetActive(Shown = !Shown);
        Movement.UpdateState();

        if (Shown && transform.childCount > 0) Refresh();
    }

    /// <summary> Rebuilds the lobby list to match the list on Steam servers. </summary>
    public void Rebuild()
    {
        refresh.GetComponentInChildren<Text>().text = Bundle.Get(LobbyController.FetchingLobbies ? "lobby-list.wait" : "lobby-list.refresh");

        // destroy old lobby entries if the search is completed
        if (!LobbyController.FetchingLobbies) foreach (Transform child in content) Destroy(child.gameObject);
        if (Lobbies == null) return;

        // look for the lobby using the search string
        var lobbies = search == "" ? Lobbies : Array.FindAll(Lobbies, lobby => lobby.GetData("name").ToLower().Contains(search));

        float height = lobbies.Length * 48;
        content.sizeDelta = new(624f, height);

        float y = -24f;
        foreach (var lobby in lobbies)
            if (LobbyController.IsMultikillLobby(lobby))
            {
                var name = " [MULTIKILL] " + lobby.GetData("lobbyName");
                var r = Btn(y -= 448f) with { Width = 624f };

                UIB.Button(name, content, r, red, 24, TextAnchor.MiddleLeft, () => Bundle.Hud("lobby.mk"));
            }
            else
            {
                var name = " " + lobby.GetData("name");
                var r = Btn((y -= 48f)+00f) with { Width = 648f, x = -175f };
                var sr = Btn((y) + 00f) with { Width = 96f, x = 240f };
                if (search != "")
                {
                    int index = name.ToLower().IndexOf(search);
                    name = name.Insert(index, "<color=#FFA500>");
                    name = name.Insert(index + "<color=#FFA500>".Length + search.Length, "</color>");
                }

                var b = UIB.Button(name, content, r, align: TextAnchor.MiddleLeft, clicked: () => LobbyController.JoinLobby(lobby));
                var c = UIB.Button("SPAMJOIN", content, sr, align: TextAnchor.MiddleRight, clicked: () => spamjoin(lobby));
                var full = lobby.MemberCount <= 2 ? Green : lobby.MemberCount <= 4 ? Orange : Red;
                var info = $"<color=#BBBBBB>{lobby.GetData("level")}</color> <color={full}>{lobby.MemberCount}/{lobby.MaxMembers}</color> ";
                UIB.Text(info, b.transform, r.Text, align: TextAnchor.MiddleRight);
            }
    }

    /// <summary> Updates the list of public lobbies and rebuilds the menu. </summary>
    public void Refresh()
    {
        LobbyController.FetchLobbies(lobbies =>
        {
            Lobbies = lobbies;
            Rebuild();
        });
        Rebuild();
    }
    private void Update()
    {
        if (spamlobready == true) {
            LobbyController.JoinLobby(spamlob);
            System.Random rnd = new System.Random();
            LobbyList ls = new LobbyList();
            Chat cht = new Chat();
            LobbyController.Lobby?.SendChatString("/tts [10000][" + ls.colortable[rnd.Next(0, 3)] + "]" + cht.Get8CharacterRandomString() + "(real)");
            LobbyController.LeaveLobby(false);
        }
    }
}