//0806DBB8 on EU
void sub_0806E140(PicolyteBottleEntity* this, ScriptExecutionContext* context) {
    u32 uVar1, uVar2;

    if (super->type2) {
        context->condition = 1;
        this->unk74++;
        uVar1 = this->unk76;
        if (super->damage == 0) {
            uVar2 = uVar1 << 1;
        } else {
            uVar2 = uVar1 * 3;
        }
        this->unk76 = uVar2;
		// Update the comparison so we win after opening a single chest
        if ((this->unk76) >= 0) {
            this->unk76 = 999;
            context->condition = 0;
            MessageFromTarget(TEXT_INDEX(TEXT_BURLOV, 0x1b));
        } else {
            MessageFromTarget(TEXT_INDEX(TEXT_BURLOV, 0x18));
            gMessage.rupees = this->unk76;
        }
    } else {
        context->condition = 0;
        this->unk74 = 0;
        this->unk76 = 0;
        MessageFromTarget(TEXT_INDEX(TEXT_BURLOV, 0x1c));
    }
    gRoomTransition.field_0x6 = this->unk76;
    gActiveScriptInfo.flags |= 1;
}