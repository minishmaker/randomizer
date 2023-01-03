.thumb
cmp	r0,#0x70
beq	shoot
cmp	r0,#0x71
beq	dig
cmp	r0,#0x72
beq	swim

ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
end:
cmp	r0,#0
beq	end2

end1:
ldr	r3,=#0x8018A19
bx	r3

end2:
ldr	r3,=#0x801889F
bx	r3

shoot:
mov	r1,#27
b	check
dig:
mov	r1,#28
b	check
swim:
mov	r1,#29
b	check

check:
ldr	r0,=#0x2002EA4
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
b	end
