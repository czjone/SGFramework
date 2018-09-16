local UIUtil = {}
local UIHelper = CS.SGF.Lua.UI.UIHelper.GetInstance();

function UIUtil.SetText( utxtGo,val )
    UIHelper.SetText(utxtGo,val);
end

function UIUtil.SetVisible(uGo,visible)
    UIHelper.SetVisible(uGo,visible);
end

function UIUtil.SetImage(uImgGo,uSprite)
    UIHelper.SetImage(uImgGo,uSprite);
end

--- PS:同时支持uinode和luanode
function UIUtil.RemoveFromeParent(node,destroy)
    local go = node.uiroot;
    if(go) then 
        assert(node:getParent() ~=nil, "node 还没有添加到父节点中.");
        UIHelper.RemoveFromeParent(go, destroy == true);
        node:onRemoveFromStage();
    else
        UIHelper.RemoveFromeParent(node, destroy == true);
    end
end

--- PS:同时支持uinode 和luanode
function UIUtil.AddChild(pNode,cNode)
    local _pnode = pNode.uiroot;
    local _cnode = cNode.uiroot;
    if(_cnode) then
        if(_pnode) then UIHelper.AddChild(_pnode,_cnode);
        else 
            assert(pNode ~= nil ,"game object pnode is nil");
            assert(_cnode ~= nil ,"game object _cnode is nil");
            UIHelper:AddChild(pNode,_cnode) 
        end
        cNode:onAddToStage();
    else
        if(_pnode) then UIHelper.AddChild(_pnode,_cnode);
        else UIHelper:AddChild(pNode,_cnode) end
    end
end

function UIUtil.LoadPrefab(nm)
    return UIHelper:LoadPrefab(nm);
end

return UIUtil;