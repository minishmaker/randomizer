.thumb
push	{lr}
@check if we talked to dampe
mov	r0,#0x69
ldr	r3,=#0x807C5F4
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	Dampe	@if we did not talk to dampe, dampe exists

@check if we got the key back
mov	r0,#0x21
ldr	r3,=#0x807C654
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	End

@check if we handed the key over
mov	r0,#0x3C
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
mov	r1,#2
and	r0,r1
cmp	r0,#2
bne	End

@spawn dampe
Dampe:
ldr	r0,=#0x80F25F8
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
End:
pop	{pc}
