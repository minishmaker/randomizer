.equ sub, item+4
.equ figurines, sub+4
.thumb
push	{lr}
@check figurine count
ldr	r0,figurines
ldr	r1,=#0x2002AF0
ldrb	r1,[r1]
cmp	r0,r1
bhi	end

@check if custom flag is set
ldr	r0,=#0x2002EA4
ldr	r1,=#9
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	end

@if not set, hand out item
ldr	r0,item
ldr	r1,sub
mov	r2,#0
ldr	r3,=#0x80A7410
mov	lr,r3
.short	0xF800
ldr	r0,=#0x2002EA4
mov	r1,#9
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800

end:
pop	{pc}

.align
.ltorg
item:
